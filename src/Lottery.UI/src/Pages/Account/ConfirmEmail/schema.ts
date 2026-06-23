import z from "zod";

export const confirmEmailPageQuerySchema = z.object({
  userId: z.uuidv4(),
  token: z.string(),
});
export type ConfirmEmailPageQuery = z.infer<typeof confirmEmailPageQuerySchema>;

export const confirmEmailRequestQuerySchema = confirmEmailPageQuerySchema;
export type ConfirmEmailRequestQuery = z.infer<
  typeof confirmEmailRequestQuerySchema
>;

export const confirmEmailResponseSchema = z.object();
export type ConfirmEmailResponse = z.infer<typeof confirmEmailResponseSchema>;
