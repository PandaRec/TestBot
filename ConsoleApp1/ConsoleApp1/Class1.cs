public class Rootobject1
{
    public int id { get; set; }
    public string title { get; set; }
    public string slug { get; set; }
    public string address { get; set; }
    public string timetable { get; set; }
    public string phone { get; set; }
    public bool is_stub { get; set; }
    public string body_text { get; set; }
    public string description { get; set; }
    public string site_url { get; set; }
    public string foreign_url { get; set; }
    public Coords coords { get; set; }
    public string subway { get; set; }
    public int favorites_count { get; set; }
    public Image[] images { get; set; }
    public int comments_count { get; set; }
    public bool is_closed { get; set; }
    public string[] categories { get; set; }
    public string short_title { get; set; }
    public string[] tags { get; set; }
    public string location { get; set; }
    public string age_restriction { get; set; }
    public bool disable_comments { get; set; }
    public bool has_parking_lot { get; set; }
}

public class Coords
{
    public float lat { get; set; }
    public float lon { get; set; }
}

public class Image
{
    public string image { get; set; }
    public Thumbnails thumbnails { get; set; }
    public Source source { get; set; }
}

public class Thumbnails
{
    public string _640x384 { get; set; }
    public string _144x96 { get; set; }
}

public class Source
{
    public string name { get; set; }
    public string link { get; set; }
}

