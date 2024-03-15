import { z } from "zod";

export const signInRequestSchema = z.object({
  username: z.string(),
  password: z.string(),
  staySignedIn: z.boolean(),
});

export const signInResponseSchema = z.object({});

export type SignInRequest = z.infer<typeof signInRequestSchema>;
export type SignInResponse = z.infer<typeof signInResponseSchema>;
