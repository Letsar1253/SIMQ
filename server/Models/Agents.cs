
namespace server.Models {

    public struct GetAgentsListData {
        public string agent_id { get; set; }
        public string description { get; set; }
        public string type { get; set; } // Todo Использовать из имеющихся
    }

    public struct GetAgentsListResponse {
        public GetAgentsListData [] data { get; set; }
    }

}
