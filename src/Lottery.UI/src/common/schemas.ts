import { z } from "zod";

type EnumLike = Record<string, string | number>;

export const userTypes = {
  Guest: "Guest",
  Basic: "Basic",
  Admin: "Admin",
  SystemAdmin: "SystemAdmin",
} as const;

export const allUserTypes = Object.keys(userTypes);

export const userTypeSchema = z.enum(userTypes);

export type UserType = z.infer<typeof userTypeSchema>;

export const allUserTypesWithLabel: ValuesAndLabels<UserType> = {
  Guest: { label: "Guest", value: "Guest" },
  Basic: { label: "Basic", value: "Basic" },
  Admin: { label: "Admin", value: "Admin" },
  SystemAdmin: { label: "SystemAdmin", value: "SystemAdmin" },
};

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
    z.enum(e),
  );
}

export const apiMessageCodeSchema = z.enum([
  "General",
  "RecentAuthRequired",
  "EmailVerificationRequired",
]);

export const apiMessageSchema = z.object({
  value: z.string(),
  code: apiMessageCodeSchema,
});

export const apiMessagesSchema = z.array(apiMessageSchema);

export const apiErrorsResponseSchema = z.object({
  messages: apiMessagesSchema.element.required().array(),
  status: z.number(),
});
export const apiSuccessResponseSchema = z.object({
  value: z.any(),
  status: z.number(),
  messages: apiMessagesSchema.element.array().optional().nullable(),
});

export const apiResponseSchema = apiSuccessResponseSchema.or(
  apiErrorsResponseSchema,
);

export type ApiMessages = z.infer<typeof apiMessagesSchema>;
export type ApiErrorsResponse = z.infer<typeof apiErrorsResponseSchema>;
export type ApiResponse = z.infer<typeof apiResponseSchema>;
export type ApiSuccessResponse = z.infer<typeof apiSuccessResponseSchema>;
//#endregion

// region ======== Item State ========
export const States = {
  Enabled: "enabled",
  Disabled: "disabled",
} as const;

export const allStates = Object.keys(States);
export const stateSchema = z.enum(States);
export type State = z.infer<typeof stateSchema>;
export const allStatesWithLabel: Record<State, ValueAndLabel<State>> = {
  enabled: { label: "Enabled", value: "enabled" },
  disabled: { label: "Disabled", value: "disabled" },
};
//#endregion

// region ======== Game State ========
export const GameStatuses = {
  Future: "future",
  Open: "open",
  Closed: "closed",
  Resulted: "resulted",
} as const;
export const allGameStatuses = Object.keys(GameStatuses);
export const gameStatusSchema = camelCaseEnum(GameStatuses);

export type GameStatus = z.infer<typeof gameStatusSchema>;
export const allGameStatusesWithLabels: ValuesAndLabels<GameStatus> = {
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

export const sortDirectionSchema = z.enum(SortDirections);

export type SortDirection = z.infer<typeof sortDirectionSchema>;

export const allSortDirectionsWithLabel: ValuesAndLabels<SortDirection> = {
  asc: { label: "Ascending", value: "asc" },
  desc: { label: "Descending", value: "desc" },
};
//#endregion
