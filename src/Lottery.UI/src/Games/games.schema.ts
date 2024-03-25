import { z } from "zod";
import {
  pagedRequestSchema,
  pagedResponseSchema,
  sortDirectionSchema,
} from "../common/schemas";

export const GameStates = {
  Future: "future",
  CanEnter: "canEnter",
  Resulted: "resulted",
  Closed: "closed",
} as const;

export const allGameStates = Object.keys(GameStates);

export const gameStateSchema = z.nativeEnum(GameStates);
export type GameState = z.infer<typeof gameStateSchema>;

export const LabelledGameStates: Record<
  GameState,
  { label: string; value: GameState }
> = {
  canEnter: { label: "Current Games", value: "canEnter" },
  future: { label: "Future Games", value: "future" },
  resulted: { label: "Past Games", value: "resulted" },
  closed: { label: "Closed Games", value: "closed" },
};

export const SortByValues = {
  DrawTime: "drawTime",
  StartTime: "startTime",
  CloseTime: "closeTime",
} as const;

export const allSortByValues = Object.keys(SortByValues);

export const sortBySchema = z.nativeEnum(SortByValues);

export type SortBy = z.infer<typeof sortBySchema>;

export const LabelledSortByValues: Record<
  SortBy,
  { label: string; value: SortBy }
> = {
  drawTime: { label: "Draw Time", value: "drawTime" },
  startTime: { label: "Start Time", value: "startTime" },
  closeTime: { label: "Close Time", value: "closeTime" },
};

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
  closeTime: z.string().pipe(z.coerce.date()),
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
