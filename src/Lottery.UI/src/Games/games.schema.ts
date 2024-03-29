import { z } from "zod";
import {
  pagedRequestSchema,
  pagedResponseSchema,
  sortDirectionSchema,
} from "../common/schemas";

export const GameStates = {
  Future: "future",
  Open: "open",
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
  open: { label: "Current Games", value: "open" },
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

// shared validation functions

export function validateSelectionsRequiredForEntry(
  {
    selectionsRequiredForEntry,
    maxSelections,
  }: { selectionsRequiredForEntry: number; maxSelections: number },
  ctx: z.RefinementCtx
) {
  if (selectionsRequiredForEntry > maxSelections) {
    ctx.addIssue({
      code: "too_big",
      path: ["selectionsRequiredForEntry"],
      message: "Cannot be greater than the maximum selections",
      maximum: maxSelections,
      type: "number",
      inclusive: true,
    });
  }
}

export function validateNumberMatchCount(
  {
    prizes,
    selectionsRequiredForEntry,
  }: {
    prizes: Array<{ numberMatchCount: number }>;
    selectionsRequiredForEntry: number;
  },
  ctx: z.RefinementCtx
) {
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
}

export function validateUniquePrizes(
  { prizes }: { prizes: Array<{ position: number; numberMatchCount: number }> },
  ctx: z.RefinementCtx
) {
  prizes.forEach((prize, i) => {
    if (prizes.filter((p) => p.position === prize.position).length > 1) {
      ctx.addIssue({
        code: "custom",
        message: "Same position used multiple times",
        path: [`prizes`, i, `position`],
      });
    }
    if (
      prizes.filter((p) => p.numberMatchCount === prize.numberMatchCount)
        .length > 1
    ) {
      ctx.addIssue({
        code: "custom",
        message: "Same match count used multiple times",
        path: [`prizes`, i, `numberMatchCount`],
      });
    }
  });
}

export function validatePrizesStartFromOne(
  { prizes }: { prizes: Array<{ position: number }> },
  ctx: z.RefinementCtx
) {
  const indexed = prizes
    .map((prize, index) => ({ prize, index }))
    .sort((a, b) => a.prize.position - b.prize.position);
  const min = indexed[0];
  if (min.prize.position !== 1) {
    ctx.addIssue({
      code: "custom",
      message: "A prize for first position is required",
      path: ["prizes", min.index, "position"],
    });
  }
}

export function validatePrizesSequential(
  { prizes }: { prizes: Array<{ position: number }> },
  ctx: z.RefinementCtx
) {
  const indexed = prizes
    .map((prize, index) => ({ prize, index }))
    .sort((a, b) => a.prize.position - b.prize.position);
  let prev = indexed[0].prize.position - 1;
  indexed.forEach(({ prize, index }) => {
    if (prize.position != prev + 1) {
      ctx.addIssue({
        code: "custom",
        message: "Prize positions should run in sequence",
        path: ["prizes", index, "position"],
      });
    }
    prev = prize.position;
  });
}
