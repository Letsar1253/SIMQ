using server.Models;
using SimQCore.Modeller;
using TaskStatus = server.Models.TaskStatus;

namespace server.Services {

    public static class TasksService {
        private readonly static uint MAX_RUNNING_TASKS = 5;

        private static uint TaskIdCounter = 0;

        private static bool IsMaxModellating {
            get {
                return StorageService.SimulationTasks.Values
                    .Select( elem => elem.Status == TaskStatus.Modelling )
                    .Count() >= MAX_RUNNING_TASKS;
            }
        }

        private static Queue<SimulationTask> tasksQueue = [];

        private static void RunWithCallback( SimulationTask simulationTask ) {
            simulationTask
                .StartModelling()
                .ContinueWith( task => {
                    string problemName = simulationTask.Problem.Name;

                    Guid guid = Guid.NewGuid();
                    Result newResult = new() {
                        creation_time = DateTime.Now,
                        task_id = simulationTask.Id,
                        text = simulationTask.Results
                    };

                    bool hasResults = StorageService.Results.ContainsKey(problemName);
                    if( hasResults ) {
                        StorageService.Results[problemName].Add( guid, newResult );
                    } else {
                        StorageService.Results.Add( problemName,
                            new( [
                                KeyValuePair.Create( guid, newResult )
                            ] )
                        );
                    }

                    return task;
                } )
                .ContinueWith( _ => {
                    if( tasksQueue.Count > 0 ) {
                        RunWithCallback( tasksQueue.Dequeue() );
                    }
                } );
        }

        public static GetTaskInfoResponse GetTaskInfo(uint task_id) {
            SimulationTask task = StorageService.SimulationTasks[task_id];

            return new GetTaskInfoResponse() {
                data = new GetTaskInfoData()
                {
                    task_id = task_id,
                    status = task.Status,
                    started = task.StartTime,
                    finished = task.EndTime
                }
            };
        }

        public static GetTaskListResponse GetTaskList() {
            GetTaskListDataItem[] list = StorageService.SimulationTasks.Select(
                elem => new GetTaskListDataItem() {
                    task_id = elem.Key,
                    status = elem.Value.Status
                }
            ).ToArray();
            return new GetTaskListResponse() { data = list };
        }

        public static void StopTask(uint task_id) {
            StorageService.SimulationTasks [task_id].StopModelling();
            StorageService.SimulationTasks.Remove( task_id );
        }

        public static uint AddTask( CreateTaskData taskProperties ) {
            Problem problem = StorageService.Problems[taskProperties.problem_name];

            SimulationTask newTask = new( problem, TaskIdCounter );
            StorageService.SimulationTasks.Add( TaskIdCounter, newTask );

            if( IsMaxModellating ) {
                tasksQueue.Enqueue( newTask );
            } else {
                RunWithCallback( newTask );
            }

            return TaskIdCounter++;
        }
    }
}
