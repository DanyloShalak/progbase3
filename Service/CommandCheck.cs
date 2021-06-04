using System;

namespace Service
{
    public static class CommandCheck
    {
        public static void CheckLog(string command) //log {login} {password}
        {
            if(command.Split(' ').Length != 3)
            {
                throw new Exception("Error: Incorrect entered command 'log'");
            }
        }
    }
}