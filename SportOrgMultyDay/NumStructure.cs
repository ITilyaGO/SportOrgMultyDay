using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportOrgMultyDay
{
    internal class NumStructure
    {
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Control
    {
        public string code { get; set; }
        public int length { get; set; }
        public string @object { get; set; }
    }

    public class Course
    {
        public int bib { get; set; }
        public double climb { get; set; }
        public List<Control> controls { get; set; }
        public int corridor { get; set; }
        public string id { get; set; }
        public int length { get; set; }
        public string name { get; set; }
        public string @object { get; set; }
    }

    public class Data
    {
        public string chief_referee { get; set; }
        public string description { get; set; }
        public string end_datetime { get; set; }
        public string location { get; set; }
        public int race_type { get; set; }
        public int relay_leg_count { get; set; }
        public string secretary { get; set; }
        public string start_datetime { get; set; }
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Group
    {
        public int __type { get; set; }
        public int count_finished { get; set; }
        public int count_person { get; set; }
        public string course_id { get; set; }
        public int first_number { get; set; }
        public string id { get; set; }
        public bool is_any_course { get; set; }
        public string long_name { get; set; }
        public int max_age { get; set; }
        public int max_time { get; set; }
        public int max_year { get; set; }
        public int min_age { get; set; }
        public int min_year { get; set; }
        public string name { get; set; }
        public string @object { get; set; }
        public int order_in_corridor { get; set; }
        public int price { get; set; }
        public Ranking ranking { get; set; }
        public int relay_legs { get; set; }
        public int sex { get; set; }
        public int start_corridor { get; set; }
        public int start_interval { get; set; }
    }

    public class Organization
    {
        public string code { get; set; }
        public string contact { get; set; }
        public int count_person { get; set; }
        public string country { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string @object { get; set; }
        public string region { get; set; }
    }

    public class Person
    {
        public int bib { get; set; }
        public string birth_date { get; set; }
        public int card_number { get; set; }
        public string comment { get; set; }
        public string group_id { get; set; }
        public string id { get; set; }
        public bool is_out_of_competition { get; set; }
        public bool is_paid { get; set; }
        public bool is_personal { get; set; }
        public bool is_rented_card { get; set; }
        public string name { get; set; }
        public object national_code { get; set; }
        public string @object { get; set; }
        public string organization_id { get; set; }
        public int qual { get; set; }
        public int sex { get; set; }
        public int start_group { get; set; }
        public int start_time { get; set; }
        public string surname { get; set; }
        public object world_code { get; set; }
        public int year { get; set; }
    }

    public class Rank
    {
        public bool is_active { get; set; }
        public string max_place { get; set; }
        public object max_time { get; set; }
        public int percent { get; set; }
        public int qual { get; set; }
        public bool use_scores { get; set; }
    }

    public class Ranking
    {
        public bool is_active { get; set; }
        public List<Rank> rank { get; set; }
        public int rank_scores { get; set; }
    }

    public class Root
    {
        public List<Course> courses { get; set; }
        public Data data { get; set; }
        public List<Group> groups { get; set; }
        public string id { get; set; }
        public string @object { get; set; }
        public List<Organization> organizations { get; set; }
        public List<Person> persons { get; set; }
        public List<object> results { get; set; }
        public Settings settings { get; set; }
    }

    public class Settings
    {
        public bool is_corridor_minute_number { get; set; }
        public bool is_corridor_order_number { get; set; }
        public bool is_fixed_number_interval { get; set; }
        public bool is_fixed_start_interval { get; set; }
        public bool is_mix_groups { get; set; }
        public bool is_split_regions { get; set; }
        public bool is_split_start_groups { get; set; }
        public bool is_split_teams { get; set; }
        public bool is_start_preparation_draw { get; set; }
        public bool is_start_preparation_numbers { get; set; }
        public bool is_start_preparation_reserve { get; set; }
        public bool is_start_preparation_time { get; set; }
        public bool marked_route_dont_dsq { get; set; }
        public bool marked_route_if_counting_lap { get; set; }
        public bool marked_route_if_station_check { get; set; }
        public bool marked_route_max_penalty_by_cp { get; set; }
        public string marked_route_mode { get; set; }
        public int marked_route_penalty_time { get; set; }
        public int marked_route_station_code { get; set; }
        public int numbers_first { get; set; }
        public int numbers_interval { get; set; }
        public double print_margin_bottom { get; set; }
        public double print_margin_left { get; set; }
        public double print_margin_right { get; set; }
        public double print_margin_top { get; set; }
        public int reserve_count { get; set; }
        public int reserve_percent { get; set; }
        public string reserve_prefix { get; set; }
        public int result_processing_fixed_score_value { get; set; }
        public string result_processing_mode { get; set; }
        public string result_processing_score_mode { get; set; }
        public int result_processing_scores_minute_penalty { get; set; }
        public string scores_array { get; set; }
        public string scores_formula { get; set; }
        public string scores_mode { get; set; }
        public bool split_printout { get; set; }
        public string split_template { get; set; }
        public int start_first_time { get; set; }
        public int start_interval { get; set; }
        public string system_assign_chip_reading { get; set; }
        public bool system_assignment_mode { get; set; }
        public string system_duplicate_chip_processing { get; set; }
        public int system_finish_cp_number { get; set; }
        public string system_finish_source { get; set; }
        public string system_port { get; set; }
        public int system_start_cp_number { get; set; }
        public string system_start_source { get; set; }
        public List<int> system_zero_time { get; set; }
        public int time_accuracy { get; set; }
        public string time_format_24 { get; set; }
    }


}
