using System.Diagnostics;
using System.Linq;
using System.Security;

namespace ConsoleApplication1
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    public class Message
    {
        public int id;
        public int len;
        public int parity;
        public string message;

        /// <summary>
        /// Default constructor for message class. It takes the message id in parameter
        /// (<paramref name="id"/>). The data to send are stored in the (<paramref name="message"/>) parameter.
        /// The constructor does the calculation for both attributes len and parity.
        /// <example>For example:
        /// <code>
        ///    var message = new Message(1, "hello");
        /// </code>
        /// This code creat a new message with id 1 and the string "hello" as data.
        /// </example>
        /// </summary>
        ///<param name="id">The id of the new message. It is used to know message's purposes</param>
        ///<param name="message">The content of the message.</param>
        public Message(int id, string message)
        {
            this.id = id;
            this.message = message;
            len = message.Length;
            for (var i = 0; i < message.Length - 1; i++)
            {
                if (message[i] == '\\' && message[i + 1] == '\"')
                {
                    len -= 1;
                }
            }

            ComputeParityBit();
        }


        /// <summary>
        /// This constructor is designed only to deserialize json strings. Please do not use.
        /// </summary>
        [JsonConstructor]
        public Message(int msgId, int msgLen, int msgParity, string msgMessage)
        {
            id = msgId;
            len = msgLen;
            parity = msgParity;
            message = msgMessage;
        }

        /// <summary>
        /// This constructor can be used to creat a new Message object from a string passed in the
        /// (<paramref name="jsonString"/>) parameter. It returns a message with id, len and parity equals to 0 with
        /// an empty message attribute if the incoming string can not be deserialized.
        /// <example>For example:
        /// <code>
        ///    var newMessage = new Message(new Message(1, "test").ToJson());
        /// </code>
        /// This code creat a new message with id 1 and the string "test" as data from a Json string.
        /// </example>
        /// </summary>
        ///<param name="jsonString">The json string to try to deserialize.</param>
        public Message(string jsonString)
        {
            try
            {
                var msg = JsonConvert.DeserializeObject<Message>(jsonString);
                id = msg.id;
                message = msg.message;
                len = msg.len;
                parity = msg.parity;
            }
            catch (JsonReaderException e)
            {
                id = 0;
                message = "";
                len = 0;
                parity = 0;
            }
        }

        /// <summary>
        /// This method compute the parity bit corresponding to the value of the message attribute.
        /// </summary>
        private void ComputeParityBit()
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var binary = bytes.Aggregate("", (current, i) => current + Convert.ToString(i, 2));

            parity = 0;
            foreach (var i in binary)
            {
                parity ^= int.Parse(i.ToString());
            }
        }

        /// <summary>
        /// This method check if the parity bit is correct. Returns true if it is.
        /// </summary>
        ///<returns>A boolean that is true if the parity bit is correct</returns>
        public bool CheckParityBit()
        {
            var bitValue = parity;
            ComputeParityBit();
            var result = bitValue == parity;
            parity = bitValue;
            return result;
        }

        /// <summary>
        /// This method check if the given message length is equals to the attribute "message" length. Returns true if it is.
        /// </summary>
        ///<returns>A boolean that is true if the length is correct</returns>
        public bool CheckMessageLen()
        {
            return message.Length == len;
        }

        /// <summary>
        /// This method check if the both the parity bit and the message length are correct. Returns true if it is.
        /// </summary>
        ///<returns>A boolean that is true if the Message check is successful</returns>
        public bool CheckMessage()
        {
            return CheckMessageLen() && CheckParityBit();
        }

        /// <summary>
        /// This method convert the Message object into a json string.
        /// <example>For example:
        /// <code>
        ///    var jsonString = new Message(1, "test").ToJson();
        /// </code>
        /// The jsonString variable contain the value {"id":1,"len":4,"parity":1,"message":"test"}
        /// </example>
        /// </summary>
        ///<returns>A json string representing the Message object.</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// This method return a boolean indicating if the (<paramref name="jsonString"/>) parameter can be deserialized
        /// into a Message object.
        /// </summary>
        ///<param name="jsonString">The json string to check.</param>
        ///<returns>A boolean which is true if the json string can be deserialized.</returns>
        public static bool IsMessage(string jsonString)
        {
            return new Message(jsonString).id != 0;
        }

        /// <summary>
        /// This method creates a new message that can be recognized for the connection to this server.
        /// (<paramref name="password"/>) is the password to send for connection.
        /// (<paramref name="hashPass"/>) have to be set to true if you want to hash
        /// (<paramref name="password"/>) value.
        /// (<paramref name="verbose"/>) value have to be set to true if you want a reply from the server.
        /// </summary>
        ///<param name="password">The password to send. If it is not already hashed use (<paramref name="hashPass"/>) = true</param>
        ///<param name="hashPass">If you want to hash the password before sending it set this value to true.</param>
        ///<param name="verbose">Set this value to true if you want a reply from the server.</param>
        ///<returns>A boolean which is true if the json string can be deserialized.</returns>
        public static string CreateConnectionMessage(string password, int verbose = 1, bool hashPass = false)
        {
            var hashPassword = hashPass ? UdpSocket.CryptPass(password) : password;

            var messageToSend = "{" + '"' + "password" + '"' + " : " + '"' + hashPassword + '"' + ", " + '"' +
                                "verbose" + '"' + " : " +
                                verbose + "}";
            return messageToSend;
        }

    }
}