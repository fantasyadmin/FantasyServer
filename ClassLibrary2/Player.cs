//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClassLibrary2
{
    using System;
    using System.Collections.Generic;
    
    public partial class Player
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Player()
        {
            this.Listed_in = new HashSet<Listed_in>();
        }
    
        public string nickname { get; set; }
        public int user_id { get; set; }
        public string picture { get; set; }
        public int player_score { get; set; }
        public int games_played { get; set; }
        public int total_wins { get; set; }
        public int total_goals_scored { get; set; }
        public int total_assists { get; set; }
        public int total_pen_missed { get; set; }
        public int total_goals_recieved { get; set; }
        public bool league_manager { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Listed_in> Listed_in { get; set; }
        public virtual User User { get; set; }
    }
}
