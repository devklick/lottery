import { z } from "zod";

export const verifyInviteRequestQuerySchema = z.object({
  email: z.string().email(),
  token: z.string().length(128),
});

export const verifyInviteRequestSchema = z.object({
  query: verifyInviteRequestQuerySchema,
});

export const verifyInviteResponseSchema = z.object({});

export const acceptInviteRequestBodySchema = z
  .object({
    email: z.string().email(),
    username: z.string(),
    password: z.string(),
    confirmPassword: z.string(),
    token: z.string().length(128),
  })
  .refine((data) => data.password === data.confirmPassword, {
    path: ["confirmPassword"],
    message: "Passwords do not match",
  });

export const acceptInviteRequestSchema = z.object({
  body: acceptInviteRequestBodySchema,
});

export const acceptInviteResponseSchema = z.object({});

export type VerifyInviteRequestQuery = z.infer<
  typeof verifyInviteRequestQuerySchema
>;

export type VerifyInviteResponse = z.infer<typeof verifyInviteResponseSchema>;

export type AcceptInviteRequest = z.infer<typeof acceptInviteRequestSchema>;
export type AcceptInviteRequestBody = z.infer<
  typeof acceptInviteRequestBodySchema
>;

export type AcceptInviteResponse = z.infer<typeof acceptInviteResponseSchema>;
