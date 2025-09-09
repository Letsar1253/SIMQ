using SimQCore.Modeller;
using SimQCore.Statistic;

namespace server.Models {
    public enum TaskStatus {
        Waiting,   // 0
        Modelling, // 1
        Error,     // 2 
        Canceled,  // 3
        Completed  // 4
    }

    public struct CreateTaskData {
        public string problem_name { get; set; }
        public uint? max_steps { get; set; }
        public uint? max_time { get; set; }
    }

    public struct CreateTaskRequest {
        public CreateTaskData data { get; set; }
    }

    public struct GetTaskListDataItem {
        public required uint task_id { get; set; }
        public required TaskStatus status { get; set; }
    }

    public struct GetTaskInfoData {
        public required uint task_id { get; set; }
        public required TaskStatus status { get; set; }
        public DateTime? started { get; set; }
        public DateTime? finished { get; set; }
    }

    public struct GetTaskListResponse {
        public required GetTaskListDataItem [] data { get; set; }
    }

    public struct GetTaskInfoResponse {
        public required GetTaskInfoData data { get; set; }
    }

    public struct CreateTaskResponse {
        public uint task_id { get; set; }
    }

    public class SimulationTask {
        private Task? ProcessedTask;

        private string DoModelling() {
            SimulationModeller modeller = new();
            modeller.Simulate( Problem );

            string results = $"Статистика по результатам моделирования задачи \"{Problem.Name}\":";

            results += $"\nEndRealTime = {modeller.EndRealTime} (Max = {Problem.MaxRealTime})";
            results += $"\nCurrentEventsAmount = {modeller.dataCollector.CurrentEventsAmount} (Max = {Problem.MaxEventsAmount})";
            results += $"\nCurrentModelationTime = {modeller.dataCollector.CurrentModelationTime} (Max = {Problem.MaxModelationTime})";
            results += $"\nCurrentGenerationError = {modeller.dataCollector.CurrentGenerationError:E} " +
                $"(Min = {Problem.generationErrorSettings.MinGenerationError:E}) ({modeller.dataCollector.CurrentGenerationError < Problem.generationErrorSettings.MinGenerationError})";

            StatesStatistic StatesStat = new(modeller.dataCollector);
            results += StatesStat.EmpDistToString();

            Results = results;

            return Results;
        }

        public Problem Problem { get; }
        public uint Id { get; }
        public TaskStatus Status { get; private set; } = TaskStatus.Waiting;
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }
        public string? Results { get; private set; }

        public SimulationTask(Problem problem, uint id ) {
            Problem = problem;
            Id = id;
        }

        public Task StartModelling() {
            StartTime = DateTime.Now;
            Status = TaskStatus.Modelling;

            ProcessedTask = Task.Run( DoModelling );
            ProcessedTask.ContinueWith( task => {
                EndTime = DateTime.Now;

                if( task.IsFaulted ) {
                    Status = TaskStatus.Error;
                } else if( task.IsCanceled ) {
                    Status = TaskStatus.Canceled;
                } else {
                    Status = TaskStatus.Completed;
                }
            } );

            return ProcessedTask;
        }

        public void StopModelling() {
            ProcessedTask?.Dispose();
        }
    }
}
