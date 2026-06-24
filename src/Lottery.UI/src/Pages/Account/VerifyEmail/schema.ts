import z from "zod";

export const verifyEmailPageQuerySchema = z.object({
  userId: z.uuidv4(),
  token: z.string(),
});
export type VerifyEmailPageQuery = z.infer<typeof verifyEmailPageQuerySchema>;

export const verifyEmailRequestQuerySchema = verifyEmailPageQuerySchema;
export type VerifyEmailRequestQuery = z.infer<
  typeof verifyEmailRequestQuerySchema
>;

export const verifyEmailResponseSchema = z.object();
export type VerifyEmailResponse = z.infer<typeof verifyEmailResponseSchema>;
