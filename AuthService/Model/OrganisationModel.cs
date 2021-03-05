using System;

namespace AuthService.Model
{
    public class OrganisationModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}