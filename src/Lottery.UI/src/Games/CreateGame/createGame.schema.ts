import { z } from "zod";
import { stateSchema } from "../../common/schemas";

export const createGamePrizeRequestSchema = z.object({
  position: z.number().positive().min(1),
  numberMatchCount: z.number().positive(),
});

export const createGamePrizesRequestSchema = z.array(
  createGamePrizeRequestSchema
);

export const createGameRequestSchema = z
  .object({
    startTime: z.date().or(z.string()).pipe(z.coerce.date()),
    closeTime: z.date().or(z.string()).pipe(z.coerce.date()),
    drawTime: z.date().or(z.string()).pipe(z.coerce.date()),
    name: z.string().min(3).max(64),
    state: stateSchema.default("enabled"),
    maxSelections: z.number().positive().min(3).max(100),
    selectionsRequiredForEntry: z.number().positive().min(3).max(100),
    prizes: createGamePrizesRequestSchema,
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

export const createGameResponseSchema = z.object({
  id: z.string().uuid(),
  startTime: z.string().pipe(z.coerce.date()),
  closeTime: z.date().or(z.string()).pipe(z.coerce.date()),
  drawTime: z.string().pipe(z.coerce.date()),
  name: z.string(),
  selectionsRequiredForEntry: z.number(),
  selections: z.array(
    z.object({
      id: z.string().uuid(),
      selectionNumber: z.number(),
    })
  ),
  prizes: z.array(
    z.object({
      id: z.string().uuid(),
      position: z.number(),
      numberMatchCount: z.number(),
    })
  ),
});

export type CreateGamePrizeRequest = z.infer<
  typeof createGamePrizeRequestSchema
>;
export type CreateGamePrizesRequest = z.infer<
  typeof createGamePrizesRequestSchema
>;
export type CreateGameRequest = z.infer<typeof createGameRequestSchema>;
export type CreateGameResponse = z.infer<typeof createGameResponseSchema>;
