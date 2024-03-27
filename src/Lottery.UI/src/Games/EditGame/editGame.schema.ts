/*
    [Required, CompareWithOther(ComparisonType.LessThan, nameof(CloseTime))]
    public required DateTime StartTime { get; set; }

    [Required, CompareWithOther(ComparisonType.LessThanOrEqual, nameof(CloseTime))]
    public required DateTime CloseTime { get; set; }

    [Required]
    public required DateTime DrawTime { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required, CompareWithOther(ComparisonType.LessThanOrEqual, nameof(MaxSelections))]
    public int SelectionsRequiredForEntry { get; set; }

    [Required, Range(0, 100)]
    public int? MaxSelections { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ItemState State { get; set; }
*/

import { z } from "zod";
import { stateSchema } from "../../common/schemas";

export const editGameRequestBodySchema = z.object({
  startTime: z.date().or(z.string()).pipe(z.coerce.date()),
  closeTime: z.date().or(z.string()).pipe(z.coerce.date()),
  drawTime: z.date().or(z.string()).pipe(z.coerce.date()),
  state: stateSchema.default("enabled"),
  maxSelections: z.number().positive().min(3).max(100),
  selectionsRequiredForEntry: z.number().positive().min(3).max(100),
});
