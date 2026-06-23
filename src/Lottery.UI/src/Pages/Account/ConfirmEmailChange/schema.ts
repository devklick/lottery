import z from "zod";

export const confirmEmailChangePageQuerySchema = z.object({
  userId: z.uuidv4(),
  token: z.string(),
  email: z.email(),
});
export type ConfirmEmailChangePageQuery = z.infer<
  typeof confirmEmailChangePageQuerySchema
>;

export const confirmEmailChangeRequestQuerySchema =
  confirmEmailChangePageQuerySchema;

export type ConfirmEmailChangeRequestQuery = z.infer<
  typeof confirmEmailChangeRequestQuerySchema
>;

export const confirmEmailChangeResponseSchema = z.object();
export type ConfirmEmailChangeResponse = z.infer<
  typeof confirmEmailChangeResponseSchema
>;
