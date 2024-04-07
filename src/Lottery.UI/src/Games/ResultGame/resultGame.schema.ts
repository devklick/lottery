import { z } from "zod";

export const resultGameRequestRouteSchema = z.object({
  gameId: z.string().uuid(),
});

export const resultGameRequestBodySchema = z.object({
  winningSelections: z.array(
    z.object({
      selectionNumber: z.number().positive(),
    })
  ),
});

export const resultGameRequestSchema = z.object({
  route: resultGameRequestRouteSchema,
  body: resultGameRequestBodySchema,
});

export const resultGameResponseSchema = z.object({});

export type ResultGameRequestRoute = z.infer<
  typeof resultGameRequestRouteSchema
>;
export type ResultGameRequestBody = z.infer<typeof resultGameRequestBodySchema>;
export type ResultGameRequest = z.infer<typeof resultGameRequestSchema>;
export type ResultGameResponse = z.infer<typeof resultGameResponseSchema>;
