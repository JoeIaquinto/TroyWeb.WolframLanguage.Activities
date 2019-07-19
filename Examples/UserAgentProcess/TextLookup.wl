TextLookup[q_ , text_] := First[Keys[TakeLargestBy[GroupBy[FindTextualAnswer[text, q, 10, {"String", "Probability"}], First -> Last], Total, 1]]];
TextLookup["@@QUESTION@@","@@TEXT@@"]