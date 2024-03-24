import { z } from "zod";
import { gameStateSchema } from "../../common/schemas";

const getGameRequestRouteSchema = z.object({
  id: z.string().uuid(),
});

export const getGameRequestSchema = z.object({
  route: getGameRequestRouteSchema,
});

export const getGameResponseSchema = z.object({
  id: z.string().uuid(),
  startTime: z.string().pipe(z.coerce.date()),
  drawTime: z.string().pipe(z.coerce.date()),
  resultedAt: z.string().nullable().pipe(z.coerce.date()),
  name: z.string(),
  selectionsRequiredForEntry: z.number(),
  gameStatus: gameStateSchema,
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

export type GetGameRequest = z.infer<typeof getGameRequestSchema>;
export type GetGameResponse = z.infer<typeof getGameResponseSchema>;

export const createEntryRequestBodySchema = z.object({
  gameId: z.string().uuid(),
  selections: z.array(
    z.object({
      selectionNumber: z.number(),
    })
  ),
});

export const createEntryRequestSchema = z.object({
  body: createEntryRequestBodySchema,
});

export const createEntryResponseSchema = z.object({});

export type CreateEntryRequestBody = z.infer<
  typeof createEntryRequestBodySchema
>;
export type CreateEntryRequest = z.infer<typeof createEntryRequestSchema>;
export type CreateEntryResponse = z.infer<typeof createEntryResponseSchema>;
