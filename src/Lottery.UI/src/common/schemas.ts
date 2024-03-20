import { z } from "zod";

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
  Asc: "Asc",
  Desc: "Desc",
} as const;

export const allSortDirecttions = Object.keys(SortDirections);

export const sortDirectionSchema = z.nativeEnum(SortDirections);

export type SortDirection = z.infer<typeof sortDirectionSchema>;
export type PagedRequest = z.infer<typeof pagedRequestSchema>;
export type PagedResponse = z.infer<typeof pagedResponseSchema>;
