using Newtonsoft.Json;

namespace Server
{
    /*
     * This class represent the expected message to create a connection between two servers. The request id is 101 
     */
    public class ConnectionMessage
    {
        public string password;
        public int verbose;

        [JsonConstructor]
        ConnectionMessage(string password, int verbose)
        {
            this.password = password;
            this.verbose = verbose;
        }
        
        public ConnectionMessage(string jsonString)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<ConnectionMessage>(jsonString);
                password = msg.password;
                verbose = msg.verbose;
            }
            catch (JsonReaderException e)
            {
                password = "";
                verbose = 0;
            }
        }
    }
}