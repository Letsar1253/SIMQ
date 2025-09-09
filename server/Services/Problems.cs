
using server.Models;
using SimQCore.Modeller;
using SimQCore.Modeller.Models;

namespace server.Services {

    public static class ProblemsService {
        public static GetProblemInfoResponse GetProblemInfo( string problem_name ) {
            Problem problem = StorageService.Problems[problem_name];

            return new GetProblemInfoResponse() {
                data = new GetProblemInfoData() {
                    agents = problem.Agents.Select<IModellingAgent, AgentData>( agent => {
                        Type agentType = agent.GetType();
                        Dictionary<string, object?> paramemeters = agentType.GetProperties()
                            .Where( property => property.CustomAttributes.Any( attr => attr.GetType() == typeof(IsParameter) ))
                            .Select( property => KeyValuePair.Create(
                                property.Name,
                                property.GetValue( agent, null )
                            ))
                            .ToDictionary();

                        return new()
                        {
                            model_name = agentType.Name,
                            agent_id = agent.Id,
                            parameters = paramemeters
                        };
                    } ).ToArray(),
                    links = problem.Links.Select( link => KeyValuePair.Create(
                        link.Key,
                        link.Value.Select( agent => agent.Id ).ToArray()
                    ) ).ToDictionary(),
                    problem_name = problem_name,
                    results = StorageService.Results[problem_name].Keys.ToArray(),
                }
            };
        }

        public static GetProblemsListResponse GetProblemsList() {
            return new GetProblemsListResponse() {
                data = StorageService.Problems.Keys.ToArray()
            };
        }

        public static void RegisterProblem( RegisterProblemData data ) {
        }

        public static GetResultsResponse GetResults( string problem_name ) {
            return new GetResultsResponse() {
                data = StorageService.Results[problem_name]
                    .Select( elem => new GetResultData() {
                        creationTime = elem.Value.creation_time,
                        result_id = elem.Key
                    } )
                    .ToArray()
            };
        }

        public static GetResultInfoResponse GetResultInfo( string problem_name, Guid result_id ) {
            return new GetResultInfoResponse() {
                data = StorageService.Results[problem_name][result_id]
            };
        }

        public static void DeleteResult( string problem_name, Guid result_id ) {
            StorageService.Results[problem_name].Remove( result_id );
        }

        public static void DeleteProblem( string problem_name ) {
            StorageService.Results.Remove( problem_name );
        }
    }
}
