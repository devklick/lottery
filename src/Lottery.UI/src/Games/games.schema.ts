import { z } from "zod";
import {
  pagedRequestSchema,
  pagedResponseSchema,
  sortDirectionSchema,
} from "../common/schemas";

export const GameStates = {
  Future: "Future",
  CanEnter: "CanEnter",
  Resulted: "Resulted",
} as const;

export const allGameStates = Object.keys(GameStates);

export const gameStateSchema = z.nativeEnum(GameStates);

export const SortByValues = {
  DrawTime: "DrawTime",
  StartTime: "StartTime",
} as const;
export const allSortByValues = Object.keys(SortByValues);

export const sortBySchema = z.nativeEnum(SortByValues);

export const searchGamesRequestFilterSchema = z.object({
  gameStates: z.array(gameStateSchema),
  sortBy: sortBySchema,
  sortDirection: sortDirectionSchema,
  name: z.string().optional(),
});

export const searchGamesRequestSchema = pagedRequestSchema.merge(
  searchGamesRequestFilterSchema
);

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
export type SearchGamesRequestFilter = z.infer<
  typeof searchGamesRequestFilterSchema
>;
export type SearchGamesRequest = z.infer<typeof searchGamesRequestSchema>;
export type SearchGamesResponseItem = z.infer<
  typeof searchGamesResponseItemSchema
>;
export type SearchGamesResponse = z.infer<typeof searchGamesResponseSchema>;
export type GameState = z.infer<typeof gameStateSchema>;
export type SortBy = z.infer<typeof sortBySchema>;
