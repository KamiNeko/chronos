using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Fields
    {
        [JsonPropertyName("statuscategorychangedate")]
        public DateTime Statuscategorychangedate { get; set; }

        [JsonPropertyName("issuetype")]
        public Issuetype Issuetype { get; set; }

        [JsonPropertyName("timespent")]
        public int Timespent { get; set; }

        [JsonPropertyName("project")]
        public Project Project { get; set; }

        [JsonPropertyName("customfield_10230")]
        public object Customfield10230 { get; set; }

        [JsonPropertyName("fixVersions")]
        public List<FixVersion> FixVersions { get; set; }

        [JsonPropertyName("customfield_10231")]
        public object Customfield10231 { get; set; }

        [JsonPropertyName("customfield_10232")]
        public object Customfield10232 { get; set; }

        [JsonPropertyName("customfield_10233")]
        public object Customfield10233 { get; set; }

        [JsonPropertyName("resolution")]
        public object Resolution { get; set; }

        [JsonPropertyName("customfield_10234")]
        public object Customfield10234 { get; set; }

        [JsonPropertyName("customfield_10225")]
        public object Customfield10225 { get; set; }

        [JsonPropertyName("customfield_10226")]
        public object Customfield10226 { get; set; }

        [JsonPropertyName("customfield_10227")]
        public object Customfield10227 { get; set; }

        [JsonPropertyName("customfield_10228")]
        public object Customfield10228 { get; set; }

        [JsonPropertyName("customfield_10229")]
        public object Customfield10229 { get; set; }

        [JsonPropertyName("resolutiondate")]
        public object Resolutiondate { get; set; }

        [JsonPropertyName("workratio")]
        public int Workratio { get; set; }

        [JsonPropertyName("watches")]
        public Watches Watches { get; set; }

        [JsonPropertyName("lastViewed")]
        public object LastViewed { get; set; }

        [JsonPropertyName("issuerestriction")]
        public Issuerestriction Issuerestriction { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("priority")]
        public Priority Priority { get; set; }

        [JsonPropertyName("customfield_10221")]
        public object Customfield10221 { get; set; }

        [JsonPropertyName("customfield_10222")]
        public object Customfield10222 { get; set; }

        [JsonPropertyName("customfield_10223")]
        public object Customfield10223 { get; set; }

        [JsonPropertyName("customfield_10224")]
        public object Customfield10224 { get; set; }

        [JsonPropertyName("labels")]
        public List<object> Labels { get; set; }

        [JsonPropertyName("customfield_10214")]
        public object Customfield10214 { get; set; }

        [JsonPropertyName("customfield_10215")]
        public object Customfield10215 { get; set; }

        [JsonPropertyName("customfield_10216")]
        public DefaultAccount Customfield_10216 { get; set; }

        [JsonPropertyName("timeestimate")]
        public object Timeestimate { get; set; }

        [JsonPropertyName("customfield_10218")]
        public object Customfield10218 { get; set; }

        [JsonPropertyName("versions")]
        public List<object> Versions { get; set; }

        [JsonPropertyName("customfield_10219")]
        public object Customfield10219 { get; set; }

        [JsonPropertyName("issuelinks")]
        public List<object> Issuelinks { get; set; }

        [JsonPropertyName("assignee")]
        public Assignee Assignee { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("components")]
        public List<object> Components { get; set; }

        [JsonPropertyName("timeoriginalestimate")]
        public object Timeoriginalestimate { get; set; }

        [JsonPropertyName("description")]
        public object Description { get; set; }

        [JsonPropertyName("customfield_10210")]
        public object Customfield10210 { get; set; }

        [JsonPropertyName("customfield_10211")]
        public List<object> Customfield10211 { get; set; }

        [JsonPropertyName("timetracking")]
        public Timetracking Timetracking { get; set; }

        [JsonPropertyName("customfield_10005")]
        public string Customfield10005 { get; set; }

        [JsonPropertyName("customfield_10203")]
        public Customfield10203 Customfield10203 { get; set; }

        [JsonPropertyName("customfield_10204")]
        public object Customfield10204 { get; set; }

        [JsonPropertyName("customfield_10205")]
        public object Customfield10205 { get; set; }

        [JsonPropertyName("security")]
        public object Security { get; set; }

        [JsonPropertyName("attachment")]
        public List<object> Attachment { get; set; }

        [JsonPropertyName("customfield_10208")]
        public object Customfield10208 { get; set; }

        [JsonPropertyName("customfield_10209")]
        public object Customfield10209 { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("creator")]
        public Creator Creator { get; set; }

        [JsonPropertyName("reporter")]
        public Reporter Reporter { get; set; }

        [JsonPropertyName("customfield_10000")]
        public object Customfield10000 { get; set; }

        [JsonPropertyName("customfield_10200")]
        public string Customfield10200 { get; set; }

        [JsonPropertyName("customfield_10201")]
        public object Customfield10201 { get; set; }

        [JsonPropertyName("customfield_10004")]
        public object Customfield10004 { get; set; }

        [JsonPropertyName("customfield_10202")]
        public object Customfield10202 { get; set; }

        [JsonPropertyName("environment")]
        public object Environment { get; set; }

        [JsonPropertyName("duedate")]
        public object Duedate { get; set; }

        [JsonPropertyName("progress")]
        public ProgressField Progress { get; set; }

        [JsonPropertyName("votes")]
        public VotesField Votes { get; set; }

        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }

        [JsonPropertyName("worklog")]
        public Worklog Worklog { get; set; }
    }
}
