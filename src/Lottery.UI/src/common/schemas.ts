import { record, z } from "zod";

export const States = {
  Enabled: "Enabled",
  Disabled: "Disabled",
} as const;

export const AllStates = Object.keys(States);

export const stateSchema = z.nativeEnum(States);

export type State = z.infer<typeof stateSchema>;

export const pagedRequestSchema = z.object({
  page: z.number().min(1),
  limit: z.number().min(1).max(100),
});

export const pagedResponseSchema = pagedRequestSchema.extend({
  total: z.number(),
});

export const SortDirections = {
  Asc: "asc",
  Desc: "desc",
} as const;

export const allSortDirections = Object.keys(SortDirections);

export const sortDirectionSchema = z.nativeEnum(SortDirections);

export type SortDirection = z.infer<typeof sortDirectionSchema>;

export const allSortDirectionsWithLabel: Record<
  SortDirection,
  { label: string; value: SortDirection }
> = {
  asc: { label: "Ascending", value: "asc" },
  desc: { label: "Descending", value: "desc" },
};

export type PagedRequest = z.infer<typeof pagedRequestSchema>;
export type PagedResponse = z.infer<typeof pagedResponseSchema>;
