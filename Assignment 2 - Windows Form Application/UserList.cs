using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2___Windows_Form_Application
{
    public class UserList
    {
        private List<User> userList = new List<User>();

        public UserList()
        {
            //Imports login.txt as array of lines
            String[] users = File.ReadAllLines("login.txt");

            foreach (String user in users)
            {
                //Splits the line further by the separating comma, and split the birthday into day, month and year
                String[] userSplit = user.Split(',');
                String[] birthday = userSplit[5].Split('-');

                //Add user to userlist based on the information stored on current line
                userList.Add(new User(userSplit[0], userSplit[1], userSplit[2], userSplit[3], userSplit[4], new DateTime(Convert.ToInt32(birthday[2]), Convert.ToInt32(birthday[1]), Convert.ToInt32(birthday[0]))));
            }
        }

        //Gets the user with corresponding login details
        public User getUser(String username, String password)
        {
            foreach (User user in userList)
            {
                if (user.checkLogin(username, password))
                {
                    return user;
                }
            }
            return null;
        }

        public void addUser(String username, String password, String userType, String firstName, String lastName, DateTime birthday)
        {
            userList.Add(new User(username, password, userType, firstName, lastName, birthday));
        }

        //Used to check if a username already exists in the list.
        public bool checkUsername(String username)
        {
            foreach (User user in userList)
            {
                if (user.Username == username)
                {
                    return true;
                }
            }
            return false;
        }

        //Check if login exists in list
        public bool checkLogin(String username, String password)
        {
            foreach (User user in userList)
            {
                if (user.checkLogin(username, password))
                {
                    return true;
                }
            }
            return false;
        }

        //Converts user list to formatted login.txt output file.
        public void saveUsersToFile()
        {
            String[] tempFile = new string[userList.Count];
            int counter = userList.Count;
            for (int i = 0; i < userList.Count; i++)
            {
                tempFile[i] = userList[i].userFileFormat();
            }
            File.WriteAllLines("login.txt", tempFile);
        }
    }
}
