ent = Interpreter["ComputedPopularCurve"]["@@FIGURE@@"];
makeAxesFalse[ g_Graphics ] := g /. Axes :> False; 
original = makeAxesFalse[ent["Plot"]];
color = Colorize[MorphologicalComponents[Erosion[Binarize[RemoveAlphaChannel[original, White]], 1], CornerNeighbors -> False]];
inStyle = Import["@@STYLE@@"];
styled = ImageRestyle[original, inStyle];
Export["@@ORIGINAL@@", original];
Export["@@COLOR@@", color];
Export["@@STYLED@@", styled];