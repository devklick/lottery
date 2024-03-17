import { z } from "zod";

const stateSchema = z.enum(["Enabled", "Disabled"]);

export const createGamePrizeRequestSchema = z.object({
  position: z.number().positive(),
  numberMatchCount: z.number().positive(),
});

export const createGamePrizesRequestSchema = z.array(
  createGamePrizeRequestSchema
);

export const createGameRequestSchema = z
  .object({
    startTime: z.date(),
    // .superRefine((value, ctx) => {
    //   if (value == null) {
    //     ctx.addIssue({
    //       code: "custom",
    //       message: "Required",
    //     });
    //   }
    // })
    drawTime: z.date(),
    // .nullable()
    // .superRefine((value, ctx) => {
    //   if (value == null) {
    //     ctx.addIssue({
    //       code: "custom",
    //       message: "Required",
    //     });
    //   }
    // })
    name: z.string(),
    state: stateSchema.default("Enabled"),
    maxSelections: z.number().positive().max(100),
    selectionsRequiredForEntry: z.number().positive(),
    prizes: z.any(),
    // prizes: createGamePrizesSchema
  })
  .refine((data) => data.selectionsRequiredForEntry <= data.maxSelections, {
    path: ["selectionsRequiredForEntry"],
    message: "Cannot be greater than the maximum selections",
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

export type CreateGamePrizeRequest = z.infer<
  typeof createGamePrizeRequestSchema
>;
export type CreateGamePrizesRequest = z.infer<
  typeof createGamePrizesRequestSchema
>;
export type CreateGameRequest = z.infer<typeof createGameRequestSchema>;
export type CreateGameResponse = z.infer<typeof createGameResponseSchema>;
