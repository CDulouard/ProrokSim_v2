using System.Collections.Generic;
using Newtonsoft.Json;

namespace Server
{
    public class RobotCommandMessage
    {
        public Dictionary<string, float> servomotors_target_position;
        public Dictionary<string, float> cc_motors_target_speed;

        [JsonConstructor]
        public RobotCommandMessage(Dictionary<string, float> servomotorsTargetPosition, Dictionary<string, float> ccMotorsTarget)
        {
            servomotors_target_position = servomotorsTargetPosition;
            cc_motors_target_speed = ccMotorsTarget;
        }

        public RobotCommandMessage(string jsonString)
        {
            try
            {
                var jsonObj = JsonConvert.DeserializeObject<RobotDataMessage>(jsonString);
                servomotors_target_position = jsonObj.servomotors_target_position;
                cc_motors_target_speed = jsonObj.cc_motors_target_speed;
            }
            catch (JsonReaderException e)
            {
                servomotors_target_position = new Dictionary<string, float>();
                cc_motors_target_speed = new Dictionary<string, float>();
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
