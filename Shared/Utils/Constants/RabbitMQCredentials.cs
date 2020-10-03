namespace Utils.Constants
{
    public static class RabbitMQCredentials
    {
        public static string Username = "joaoxoc";
        public static string Password = "123456";
        public static string VirtualHost = "/";
        public static int Port = 5672;

        //public static string HostName = "doorman.pep.int";
        //public static string HostName = "deathlok.pep.int";
        //public static string HostName = "dazzler.pep.int";
        public static string HostName = "172.21.0.13"; //docker school_bridge network - use command: docker inspect school_bridge, if unavailable, use command: docker network connect school_bridge rabbit-docker

    }
}
