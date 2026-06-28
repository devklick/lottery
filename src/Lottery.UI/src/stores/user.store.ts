import { create } from "zustand";
import { persist } from "zustand/middleware";

import { UserType } from "../common/schemas";

interface UserStore {
  authenticated: boolean;
  sessionExpiry: Date;
  userType: UserType;

  login(userType: UserType, sessionExpiry: Date): void;
  logout(): void;
  isUserType(userType: UserType): boolean;
}

let expiryTimeout: ReturnType<typeof setTimeout> | null = null;

function scheduleExpiry(expiry: Date, onExpired: VoidFunction) {
  if (expiryTimeout) {
    clearTimeout(expiryTimeout);
  }

  const ms = expiry.getTime() - Date.now();

  if (ms <= 0) {
    onExpired();
    return;
  }

  expiryTimeout = setTimeout(() => {
    onExpired();
  }, ms);
}

export const useUserStore = create<UserStore>()(
  persist(
    (set, get) => ({
      authenticated: false,
      userType: "Guest",
      sessionExpiry: new Date(0),
      login(userType, sessionExpiry) {
        set({ authenticated: true, userType, sessionExpiry });
        scheduleExpiry(sessionExpiry, () => get().logout());
      },
      logout() {
        set({
          authenticated: false,
          userType: "Guest",
          sessionExpiry: new Date(0),
        });
        if (expiryTimeout) clearTimeout(expiryTimeout);
      },
      isUserType(userType) {
        return get().userType === userType;
      },
    }),
    {
      name: "user",
      merge(persisted, current) {
        const us = persisted as UserStore;
        if (!us) return current;
        return {
          ...current,
          ...us!,
          sessionExpiry: new Date(us.sessionExpiry),
        };
      },
      onRehydrateStorage() {
        return (state) => {
          if (!state) return;
          scheduleExpiry(state.sessionExpiry, () => state.logout());
          return state;
        };
      },
    },
  ),
);
