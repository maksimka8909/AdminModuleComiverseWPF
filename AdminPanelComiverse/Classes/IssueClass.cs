using System;

namespace AdminPanelComiverse.Classes;

public class IssueClass
{
    public int idIssue { get; set; }
    public string name { get; set; }
    public int issueNumber { get; set; }
    public string nameFile { get; set; }
    public string date { get; set; }
    public string pathRead { get; set; }
    public string pathDownload { get; set; }
}