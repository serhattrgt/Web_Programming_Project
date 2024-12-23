using Web_Programming_Proje.Models;

namespace Web_Programming_Proje.ViewModels
{
    public class ManageUserViewModel{

        public long UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string OpenAdress { get; set; } = string.Empty;
        public List<Role> Roles{ get; set; } = new List<Role>();
        public int OrderCount { get; set; }

    }
}