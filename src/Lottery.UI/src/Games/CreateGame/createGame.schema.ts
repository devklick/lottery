import { z } from "zod";
import { pagedResponseSchema } from "../../common/schemas";

const stateSchema = z.enum(["Enabled", "Disabled"]);

export const createGamePrizeRequestSchema = z.object({
  position: z.number().positive().min(1),
  numberMatchCount: z.number().positive(),
});

export const createGamePrizesRequestSchema = z.array(
  createGamePrizeRequestSchema
);

export const createGameRequestSchema = z
  .object({
    startTime: z.string().pipe(z.coerce.date()),
    drawTime: z.string().pipe(z.coerce.date()),
    name: z.string().min(3).max(64),
    state: stateSchema.default("Enabled"),
    maxSelections: z.number().positive().min(3).max(100),
    selectionsRequiredForEntry: z.number().positive().min(3).max(100),
    prizes: createGamePrizesRequestSchema,
  })
  .refine((data) => data.selectionsRequiredForEntry <= data.maxSelections, {
    path: ["selectionsRequiredForEntry"],
    message: "Cannot be greater than the maximum selections",
  })
  .superRefine(({ prizes, maxSelections }, ctx) => {
    prizes.forEach(({ numberMatchCount }, i) => {
      if (numberMatchCount > maxSelections) {
        ctx.addIssue({
          type: "number",
          code: z.ZodIssueCode.too_big,
          maximum: maxSelections,
          inclusive: true,
          message: "Cannot be greater than the maximum selections",
          path: ["prizes", i, "numberMatchCount"],
        });
      }
    });
  });

export const createGameResponseSchema = z.object({
  id: z.string().uuid(),
  startTime: z.string().pipe(z.coerce.date()),
  drawTime: z.string().pipe(z.coerce.date()),
  name: z.string(),
  selectionsRequiredForEntry: z.string(),
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

export const searchGamesRequestSchema = z.object({
  page: z.number().min(1),
  limit: z.number().min(1).max(100),
});

export const searchGamesResponseItemSchema = z.object({
  id: z.string().uuid(),
  name: z.string(),
  startTime: z.string().pipe(z.coerce.date()),
  drawTime: z.string().pipe(z.coerce.date()),
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
      position: z.number().positive(),
      numberMatchCount: z.number(),
    })
  ),
});

export const searchGamesResponseSchema = pagedResponseSchema.extend({
  items: z.array(searchGamesResponseItemSchema),
});

export type CreateGamePrizeRequest = z.infer<
  typeof createGamePrizeRequestSchema
>;
export type CreateGamePrizesRequest = z.infer<
  typeof createGamePrizesRequestSchema
>;
export type CreateGameRequest = z.infer<typeof createGameRequestSchema>;
export type CreateGameResponse = z.infer<typeof createGameResponseSchema>;
export type SearchGamesRequest = z.infer<typeof searchGamesRequestSchema>;
export type SearchGamesResponseItem = z.infer<
  typeof searchGamesResponseItemSchema
>;
export type SearchGamesResponse = z.infer<typeof searchGamesResponseSchema>;
