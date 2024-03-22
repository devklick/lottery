import { EnumLike, z } from "zod";

// region ======== Common ========
type ValueAndLabel<Value> = {
  label: string;
  value: Value;
};

type ValuesAndLabels<Value extends string> = Record<
  Value,
  ValueAndLabel<Value>
>;

export function camelCaseEnum<T extends EnumLike>(e: T) {
  return z.preprocess(
    (val) => String(val)[0].toLowerCase() + String(val).slice(1),
    z.nativeEnum(e)
  );
}
//#endregion

// region ======== Item State ========
export const States = {
  Enabled: "enabled",
  Disabled: "disabled",
} as const;

export const allStates = Object.keys(States);
export const stateSchema = z.nativeEnum(States);
export type State = z.infer<typeof stateSchema>;
export const allStatesWithLabel: Record<State, ValueAndLabel<State>> = {
  enabled: { label: "Enabled", value: "enabled" },
  disabled: { label: "Disabled", value: "disabled" },
};
//#endregion

// region ======== Game State ========
export const GameStates = {
  Future: "future",
  Open: "open",
  Closed: "closed",
  Resulted: "resulted",
} as const;
export const allGameStates = Object.keys(GameStates);
export const gameStateSchema = camelCaseEnum(GameStates);

export type GameState = z.infer<typeof gameStateSchema>;
export const allGameStatesWithLabels: ValuesAndLabels<GameState> = {
  closed: { value: "closed", label: "Closed" },
  open: { value: "open", label: "Open" },
  future: { value: "future", label: "Future" },
  resulted: { value: "resulted", label: "Resulted" },
};
//#endregion

// region ======== Pagination ========
export const pagedRequestSchema = z.object({
  page: z.number().min(1),
  limit: z.number().min(1).max(100),
});

export const pagedResponseSchema = pagedRequestSchema.extend({
  total: z.number(),
});

export type PagedRequest = z.infer<typeof pagedRequestSchema>;
export type PagedResponse = z.infer<typeof pagedResponseSchema>;
//#endregion

// region ======== Sorting ========
export const SortDirections = {
  Asc: "asc",
  Desc: "desc",
} as const;

export const allSortDirections = Object.keys(SortDirections);

export const sortDirectionSchema = z.nativeEnum(SortDirections);

export type SortDirection = z.infer<typeof sortDirectionSchema>;

export const allSortDirectionsWithLabel: ValuesAndLabels<SortDirection> = {
  asc: { label: "Ascending", value: "asc" },
  desc: { label: "Descending", value: "desc" },
};
//#endregion
