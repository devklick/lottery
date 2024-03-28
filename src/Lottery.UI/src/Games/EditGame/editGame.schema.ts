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

export const editGamePrizeRequestSchema = z.object({
  position: z.number().positive().min(1),
  numberMatchCount: z.number().positive(),
});

export const editGamePrizesRequestSchema = z.array(editGamePrizeRequestSchema);

export const editGameRequestBodySchema = z
  .object({
    startTime: z.date().or(z.string()).pipe(z.coerce.date()),
    closeTime: z.date().or(z.string()).pipe(z.coerce.date()),
    drawTime: z.date().or(z.string()).pipe(z.coerce.date()),
    state: stateSchema.default("enabled"),
    maxSelections: z.number().positive().min(3).max(100),
    selectionsRequiredForEntry: z.number().positive().min(3).max(100),
    prizes: editGamePrizesRequestSchema,
    name: z.string().min(3).max(64),
  })
  .refine((data) => data.selectionsRequiredForEntry <= data.maxSelections, {
    path: ["selectionsRequiredForEntry"],
    message: "Cannot be greater than the maximum selections",
  })
  .superRefine(({ prizes, selectionsRequiredForEntry }, ctx) => {
    prizes.forEach(({ numberMatchCount }, i) => {
      if (numberMatchCount > selectionsRequiredForEntry) {
        ctx.addIssue({
          type: "number",
          code: z.ZodIssueCode.too_big,
          maximum: selectionsRequiredForEntry,
          inclusive: true,
          message: "Cannot be greater than the number of selections per entry",
          path: ["prizes", i, "numberMatchCount"],
        });
      }
    });
  });

export const editGameRequestRouteSchema = z.object({
  id: z.string().uuid(),
});

export const editGameRequestSchema = z.object({
  route: editGameRequestRouteSchema,
  body: editGameRequestBodySchema,
});

export const editGameResponseSchema = z.object({
  id: z.string().uuid(),
});

export type EditGamePrizeRequest = z.infer<typeof editGamePrizeRequestSchema>;
export type EditGamesPrizeRequest = z.infer<typeof editGamePrizesRequestSchema>;
export type EditGameRequestBody = z.infer<typeof editGameRequestBodySchema>;
export type EditGameRequest = z.infer<typeof editGameRequestSchema>;

export type EditGameResponse = z.infer<typeof editGameResponseSchema>;
