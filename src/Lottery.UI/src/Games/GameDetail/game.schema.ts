import { z } from "zod";
import {
  States,
  camelCaseEnum,
  gameStatusSchema,
  pagedRequestSchema,
  pagedResponseSchema,
} from "../../common/schemas";

const getGameRequestRouteSchema = z.object({
  id: z.uuid(),
});

export const getGameRequestSchema = z.object({
  route: getGameRequestRouteSchema,
});

export const getGameResponseSchema = z.object({
  id: z.uuid(),
  startTime: z.string().pipe(z.coerce.date()),
  closeTime: z.string().pipe(z.coerce.date()),
  drawTime: z.string().pipe(z.coerce.date()),
  resultedAt: z.string().nullable().pipe(z.coerce.date()),
  name: z.string(),
  selectionsRequiredForEntry: z.number(),
  gameStatus: gameStatusSchema,
  state: camelCaseEnum(States),
  selections: z.array(
    z.object({
      id: z.uuid(),
      selectionNumber: z.number(),
    }),
  ),
  results: z.array(
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

export type GetGameRequest = z.infer<typeof getGameRequestSchema>;
export type GetGameResponse = z.infer<typeof getGameResponseSchema>;

export const createEntryRequestBodySchema = z.object({
  gameId: z.uuid(),
  selections: z.array(
    z.object({
      selectionNumber: z.number(),
    }),
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

export const editEntryRequestRouteSchema = z.object({
  entryId: z.uuid(),
});
export const editEntryRequestBodySchema = z.object({
  selections: z.array(
    z.object({
      selectionNumber: z.number().positive(),
    }),
  ),
});
export const editEntryRequestSchema = z.object({
  route: editEntryRequestRouteSchema,
  body: editEntryRequestBodySchema,
});
export const editEntryResponseSchema = z.object({});
export type EditEntryRequest = z.infer<typeof editEntryRequestSchema>;
export type EditEntryRequestBody = z.infer<typeof editEntryRequestBodySchema>;
export type EditEntryResponse = z.infer<typeof editEntryResponseSchema>;

export const getEntriesRequestQuerySchema = pagedRequestSchema.extend({
  gameId: z.string(),
});
export const getEntriesRequestSchema = z.object({
  query: getEntriesRequestQuerySchema,
});
export const getEntriesResponseItemSchema = z.object({
  gameId: z.uuid(),
  id: z.uuid(),
  selections: z.array(
    z.object({
      id: z.uuid(),
      selectionNumber: z.number(),
    }),
  ),
  prize: z
    .object({
      id: z.uuid(),
      position: z.number(),
      numberMatchCount: z.number(),
    })
    .nullable(),
});
export const getEntriesResponseSchema = pagedResponseSchema.extend({
  items: z.array(getEntriesResponseItemSchema),
});

export type GetEntriesRequestQuery = z.infer<
  typeof getEntriesRequestQuerySchema
>;
export type GetEntriesRequest = z.infer<typeof getEntriesRequestSchema>;
export type GetEntriesResponseItem = z.infer<
  typeof getEntriesResponseItemSchema
>;
export type GetEntriesResponse = z.infer<typeof getEntriesResponseSchema>;
export type EntryPrize = GetEntriesResponseItem["prize"];
