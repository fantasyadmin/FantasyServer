
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
    using System.Web.Http.Cors;

    public partial class Active_in
    {
        EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
        public int user_id { get; set; }

        public byte attending { get; set; }

        public int pen_missed { get; set; }

        public int goals_recieved { get; set; }

        public byte apporval_status { get; set; }

        public string match_color { get; set; }

        public int assists { get; set; }

        public int goals_scored { get; set; }

        public int match_id { get; set; }

        public int league_id { get; set; }

        public int wins { get; set; }



        public virtual Match Match { get; set; }

        public virtual User User { get; set; }

    }

}
