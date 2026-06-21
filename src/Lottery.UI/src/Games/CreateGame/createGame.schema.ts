import { z } from "zod";

import { stateSchema } from "../../common/schemas";
import {
  validateNumberMatchCount,
  validatePrizesSequential,
  validatePrizesStartFromOne,
  validateSelectionsRequiredForEntry,
  validateUniquePrizes,
} from "../games.schema";

export const createGamePrizeRequestSchema = z.object({
  position: z.number().positive().min(1),
  numberMatchCount: z.number().positive(),
});

export const createGamePrizesRequestSchema = z.array(
  createGamePrizeRequestSchema,
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
  .superRefine(validateSelectionsRequiredForEntry)
  .superRefine(validateNumberMatchCount)
  .superRefine(validateUniquePrizes)
  .superRefine(validatePrizesStartFromOne)
  .superRefine(validatePrizesSequential);

export const createGameResponseSchema = z.object({
  id: z.uuid(),
  startTime: z.string().pipe(z.coerce.date()),
  closeTime: z.date().or(z.string()).pipe(z.coerce.date()),
  drawTime: z.string().pipe(z.coerce.date()),
  name: z.string(),
  selectionsRequiredForEntry: z.number(),
  selections: z.array(
    z.object({
      id: z.uuid(),
      selectionNumber: z.number(),
    }),
  ),
  prizes: z.array(
    z.object({
      id: z.uuid(),
      position: z.number(),
      numberMatchCount: z.number(),
    }),
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
