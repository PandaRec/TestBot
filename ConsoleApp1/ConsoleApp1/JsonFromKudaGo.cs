
        public class Rootobject
        {
            public int count { get; set; }
            public string next { get; set; }
            public object previous { get; set; }
            public Result[] results { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string title { get; set; }
            public string slug { get; set; }
            public string address { get; set; }
            public string phone { get; set; }
            public string site_url { get; set; }
            public string subway { get; set; }
            public bool is_closed { get; set; }
            public string location { get; set; }
            public bool has_parking_lot { get; set; }
        }
