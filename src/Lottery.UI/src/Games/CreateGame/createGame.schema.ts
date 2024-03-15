import { z } from "zod";

const stateSchema = z.enum(["Enabled", "Disabled"]);

export const createGameRequestSchema = z.object({
  startTime: z
    .date()
    .nullable()
    .superRefine((value, ctx) => {
      if (value == null) {
        ctx.addIssue({
          code: "custom",
          message: "Required",
        });
      }
    }),
  drawTime: z
    .date()
    .nullable()
    .superRefine((value, ctx) => {
      if (value == null) {
        ctx.addIssue({
          code: "custom",
          message: "Required",
        });
      }
    }),
  name: z.string(),
  state: stateSchema,
  maxSelections: z.number().positive().max(100),
  selectionsRequiredForEntry: z.number().positive(),
  prizes: z.array(
    z.object({
      position: z.number(),
      numberMatchCount: z.number(),
    })
  ),
});

export const createGameResponseSchema = z.object({
  id: z.string().uuid(),
  startTime: z.date(),
  drawTime: z.date(),
  name: z.string(),
  selectionsRequiredForEntry: z.string(),
  selections: z.array(
    z.object({
      id: z.string().uuid(),
      selectionNumber: z.number(),
    })
  ),
  prizes: z.array(
    z.object({
      id: z.string().uuid(),
      position: z.number(),
      numberMatchCount: z.number(),
    })
  ),
});

export type CreateGameRequest = z.infer<typeof createGameRequestSchema>;
export type CreateGameResponse = z.infer<typeof createGameResponseSchema>;
