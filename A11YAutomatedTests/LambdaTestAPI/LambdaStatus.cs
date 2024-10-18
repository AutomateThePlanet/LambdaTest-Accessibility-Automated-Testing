namespace A11YAutomatedTests;
public enum LambdaStatus
{
    [System.ComponentModel.Description("passed")]
    PASSED,
    [System.ComponentModel.Description("failed")]
    FAILED,
    [System.ComponentModel.Description("error")]
    ERROR,
    [System.ComponentModel.Description("unknown")]
    UNKNOWN,
    [System.ComponentModel.Description("ignored")]
    IGNORED,
    [System.ComponentModel.Description("skipped")]
    SKIPPED
}
