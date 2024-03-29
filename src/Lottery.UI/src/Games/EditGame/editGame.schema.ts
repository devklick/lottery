import { z } from "zod";
import { stateSchema } from "../../common/schemas";
import {
  validateNumberMatchCount,
  validatePrizesSequential,
  validatePrizesStartFromOne,
  validateSelectionsRequiredForEntry,
  validateUniquePrizes,
} from "../games.schema";

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
  .superRefine(validateSelectionsRequiredForEntry)
  .superRefine(validateNumberMatchCount)
  .superRefine(validateUniquePrizes)
  .superRefine(validatePrizesStartFromOne)
  .superRefine(validatePrizesSequential);

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
