using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2___Windows_Form_Application
{
    public enum typeEnum
    {
        View,
        Edit
    }

    public class User
    {
        private String username;
        private String password;
        private typeEnum userType;
        private String firstName;
        private String lastName;
        private DateTime birthday;

        public User(String username, String password, String userType, String firstName, String lastName, DateTime birthday)
        {
            this.username = username;
            this.password = password;
            if (userType == "View")
            {
                this.userType = typeEnum.View;
            }
            else if (userType == "Edit")
            {
                this.userType = typeEnum.Edit;
            }
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthday = birthday;
        }

        public typeEnum UserType
        {
            get { return userType; }
        }

        public string Username
        {
            get { return username; }
        }

        //Formats the file according to the sample login.txt provided on the assignment specs
        public string userFileFormat()
        {
            return $"{username},{password},{userType},{firstName},{lastName},{birthday.Day}-{birthday.Month}-{birthday.Year}";
        }

        public bool checkLogin(String username, String password)
        {
            return username == this.username && password == this.password;
        }
    }
}
