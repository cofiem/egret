
using System;
using System.Collections.Generic;
using System.Linq;
using Egret.Cli.Models.Results;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Egret.Cli.Models
{
    /// <summary>
    /// This segment-level expectation checks if any returned result contains the target label.
    /// This expectation is automatically generated for any event-level expectations so 
    /// we can measure segment-level performance easily.
    /// </summary>
    public class LabelPresent : AggregateExpectation
    {
        private const string AssertionName = "Label present in segment";
        private string name;

        public string Label { get; init; }
        public override bool Match { get; init; } = true;

        public override bool IsPositiveAssertion { get; } = true;

        public override string Name
        {
            get => name ?? $"Segment has {Label}";
            init => name = value;
        }


        public override IEnumerable<ExpectationResult> Test(IReadOnlyList<NormalizedResult> actualEvents, Suite suite)
        {
            var aliases = suite.LabelAliases;
            var aliasNames = aliases.Length > 0 ? ". Also checked aliases: " + aliases.JoinIntoSetNotation() : string.Empty;
            Seq<Assertion> errors = Empty;
            foreach (var result in actualEvents)
            {
                // test if result even has a label
                switch (result.Label.Case)
                {
                    case KeyedValue<string> label:
                        // now test if label matches
                        var test = aliases.MatchThroughAliases(label.Value, Label, StringComparison.InvariantCultureIgnoreCase);
                        if (((IExpectation)this).Matches(test.IsSome))
                        {
                            var (first, second) = (ValueTuple<string, string>)test;
                            // success!
                            yield return new ExpectationResult(this, new SuccessfulAssertion(AssertionName, label.Key, $"`{label.Value}` = `{second}`"));
                            yield break;
                        }
                        else
                        {
                            errors = errors.Add(new FailedAssertion(Expectation.NameAssertionName, label.Key, $"value `{label.Value}` ≠ expected `{Label}{aliasNames}`"));
                        }

                        break;
                    case Seq<string> error:
                        errors = errors.Add(new ErrorAssertion(Expectation.NameAssertionName, null, error));
                        break;
                }
            }

            errors = errors.Add(new FailedAssertion(AssertionName, null, $"No result produced a label that matched `{Label}`"));
            yield return new ExpectationResult(
                this,
                errors.ToArray());

        }
    }

}

