namespace A11YAutomatedTests.Accessibility;


public class LambdaTestAutomationIssues
{
    public int TotalIssues { get; set; }
    public int CriticalIssues { get; set; }
    public int MinorIssues { get; set; }
    public int ModerateIssues { get; set; }
    public int SeriousIssues { get; set; }
    public string TestId { get; set; }
    public string ScanType { get; set; }
    public int OrgId { get; set; }
    public string Status { get; set; }
    public int UserId { get; set; }
    public string TestName { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string TestType { get; set; }
}

public class AllIssue
{
    public bool BestPractice { get; set; }
    public string Class { get; set; }
    public string Description { get; set; }
    public string FailureSummary { get; set; }
    public string HelpUrl { get; set; }
    public string Html { get; set; }
    public string HtmlTagName { get; set; }
    public string Id { get; set; }
    public string Impact { get; set; }
    public string IssueId { get; set; }
    public string Name { get; set; }
    public bool NeedsReview { get; set; }
    public string Target { get; set; }
    public string Url { get; set; }
    public string WcagGuideline { get; set; }
    public List<string> WcagVersion { get; set; }
    public string Xpath { get; set; }
}

public class FailedRule
{
    public int Count { get; set; }
    public string Mode { get; set; }
    public string Name { get; set; }
}

public class IssueSummary
{
    public int BestPractices { get; set; }
    public int Critical { get; set; }
    public int Minor { get; set; }
    public int Moderate { get; set; }
    public int NeedsReview { get; set; }
    public int Serious { get; set; }
}

public class NeedsReview
{
    public int Count { get; set; }
    public string Mode { get; set; }
    public string Name { get; set; }
}

public class Source
{
    public string ProductName { get; set; }
    public string ProductVersion { get; set; }
}

public class ScanJson
{
    public string TestId { get; set; }
    public string Url { get; set; }
    public int IssueCount { get; set; }
    public string ScanId { get; set; }
    public int AccessibilityScore { get; set; }
    public string Standard { get; set; }
    public DateTime StartDate { get; set; }
    public long CurrentTimestamp { get; set; }
    public string ScanType { get; set; }
    public string AxeVersion { get; set; }
    public string ExtensionVersion { get; set; }
    public bool BestPracticesEnabled { get; set; }
    public bool NeedsReviewEnabled { get; set; }
    public List<AllIssue> AllIssues { get; set; }
    public List<FailedRule> FailedRules { get; set; }
    public IssueSummary IssueSummary { get; set; }
    public List<NeedsReview> NeedsReview { get; set; }
    public Source Source { get; set; }
}

public class Root
{
    public LambdaTestAutomationIssues TestInfo { get; set; }
    public List<ScanJson> ScanJson { get; set; }
}
