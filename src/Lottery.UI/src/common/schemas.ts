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

export type PagedRequest = z.infer<typeof pagedRequestSchema>;
export type PagedResponse = z.infer<typeof pagedResponseSchema>;
