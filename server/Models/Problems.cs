
namespace server.Models {

    public struct RegisterProblemData {
        public string name { get; set; }
        public string agents { get; set; }
        public string links { get; set; }
    }

    public struct RegisterProblemRequest {
        public RegisterProblemData data { get; set; }
    }


    public struct AgentData {
        public string model_name { get; set; }
        public string agent_id { get; set; }
        public Dictionary<string, object?> parameters { get; set; }
    }
    public struct GetProblemInfoData {
        // Todo (идентификатор, наименование, параметры задачи, список агентов (идентификатор агента, идентификатор класса, параметры), список связей, [идентификаторы результатов, идентификаторы асинхронных задач])
        public string problem_name { get; set; }
        public AgentData[] agents { get; set; }
        public Dictionary<string, string []> links { get; set; }
        public Guid[] results { get; set; }
    }

    public struct GetProblemsListResponse {
        public string [] data { get; set; }
    }

    public struct GetProblemInfoResponse {
        public GetProblemInfoData data { get; set; }
    }

    public struct GetResultData {
        public Guid result_id { get; set; }
        public DateTime creationTime { get; set; }
    }

    public struct GetResultsResponse {
        public GetResultData [] data { get; set; }
    }

    public struct GetResultInfoResponse {
        public Result data { get; set; }
    }

    public struct Result {
        public string? text { get; set; }
        public DateTime creation_time { get; set; }
        public uint task_id { get; set; }
    }
}
