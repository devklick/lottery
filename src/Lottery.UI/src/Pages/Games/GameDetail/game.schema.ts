import { z } from "zod";

import {
  States,
  camelCaseEnum,
  gameStatusSchema,
  pagedRequestSchema,
  pagedResponseSchema,
} from "../../../common/schemas";

const getGameRequestRouteSchema = z.object({
  id: z.uuid(),
});

export const getGameRequestSchema = z.object({
  route: getGameRequestRouteSchema,
});

export const gameObjectSchema = z.object({
  id: z.uuid().describe("The unique identifier for the game"),
  startTime: z
    .string()
    .pipe(z.coerce.date())
    .describe(
      "The date and time at which the game will allow players to submit entries",
    ),
  closeTime: z
    .string()
    .pipe(z.coerce.date())
    .describe(
      "The date and time at which players will no longer be able to submit entries",
    ),
  drawTime: z
    .string()
    .pipe(z.coerce.date())
    .describe("The date and time at which the results process will begin"),
  resultedAt: z
    .string()
    .nullable()
    .pipe(z.coerce.date())
    .describe("The date and time at which the results where drawn"),
  name: z.string().describe("A user-friendly name for the game"),
  selectionsRequiredForEntry: z
    .number()
    .describe(
      "The count of numbers that a player must pick to submit their entry",
    ),
  maxEntriesPerPlayer: z
    .number()
    .describe(
      "The maximum number of entries a single player can have in the game",
    ),
  gameStatus: gameStatusSchema.describe("The current status"),
  state: camelCaseEnum(States).describe("Whether or not the game is enabled"),
  selections: z.array(
    z.object({
      id: z.uuid().describe("The unique identifier for this selection"),
      selectionNumber: z
        .number()
        .describe("A number that is available in the game for players to pick"),
    }),
  ),
  results: z.array(
    z.object({
      id: z.uuid().describe("The unique identifier for this result"),
      selectionNumber: z.number().describe("The number that was drawn"),
    }),
  ),
  prizes: z.array(
    z.object({
      id: z.uuid().describe("The unique identifier for this prize"),
      position: z
        .number()
        .describe(
          "The position of this prize in relation to the other prizes. Higher position can be considered higher value",
        ),
      numberMatchCount: z
        .number()
        .describe(
          "The count of matching numbers required to receive this prize",
        ),
    }),
  ),
});

export const getGameResponseSchema = gameObjectSchema.clone();

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
