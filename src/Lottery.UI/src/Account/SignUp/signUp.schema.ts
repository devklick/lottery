import { z } from "zod";

export const signUpRequestSchema = z.object({
  email: z.string().email(),
  username: z.string(),
  password: z.string(),
});

export const signUpResponseSchema = z.object({});

export type SignUpRequest = z.infer<typeof signUpRequestSchema>;
export type SignUpResponse = z.infer<typeof signUpResponseSchema>;
