

namespace Web_Programming_Proje.ViewModels
{ 
    public class UserProfileViewModel{   

        public long UserID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
    }

}