using System.Text.Json.Serialization;

namespace HeyClicky.Core.Worker;

public sealed record PlanSnapshot(string Plan, PlanUsage Usage, string BillingEnforcement, string? Tier = null, string? TierSource = null, string? TeamName = null, PlanSubscription? Subscription = null, int? UnlimitedAgentsCap = null, int? ProAgentsCap = null, int? MaxAgentsCap = null, bool? ProYearlyAvailable = null, bool? MaxYearlyAvailable = null, bool? IsFailOpenFallback = null)
{
	[JsonIgnore]
	public string EffectiveTier => "max";

	[JsonIgnore]
	public bool IsFreeUserOverMessageCap => false;
}
